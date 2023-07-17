using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WEbApiAutores.DTOs;
using WEbApiAutores.Servicios;

namespace WEbApiAutores.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class CuentasController:ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly HashService hashService;
        private readonly IDataProtector dataProtector;

        public CuentasController( UserManager<IdentityUser> userManager, 
                                    IConfiguration configuration,
                                    SignInManager<IdentityUser> signInManager,
                                    IDataProtectionProvider dataProtectionProvider,
                                    HashService hashService)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.hashService = hashService;
            dataProtector =  dataProtectionProvider.CreateProtector("valor_unico_y_quizas_secreto"); // Esto es parte de la llave
            
        }

        //[HttpGet("Hash/{textoPlano}")]
        //public ActionResult RealizarHash(string textoPlano) 
        //{
        //    var resultado1 = hashService.Hash(textoPlano);
        //    var resultado2 = hashService.Hash(textoPlano);

        //    return Ok( new
        //    {
        //        textoPlano = textoPlano,
        //        resultado1 = resultado1,
        //        resultado2 = resultado2 
        //    });
        //}

        //[HttpGet("Encriptar")]
        //public ActionResult Encriptar() 
        //{
        //    var textoPlano = "Gaby Cornejo";
        //    var textoCifrado = dataProtector.Protect(textoPlano);
        //    var textoDesencriptado = dataProtector.Unprotect(textoCifrado);

        //    return Ok(new 
        //    {
        //        textoPlano = textoPlano,
        //        textoCifrado = textoCifrado,
        //        textoDesencriptado = textoDesencriptado
        //    });
        //}

        //[HttpGet("EncriptarPorTiempo")]
        //public ActionResult EncriptarPorTiempo()
        //{
        //    var protectorLimitadoPorTiempo = dataProtector.ToTimeLimitedDataProtector();

        //    var textoPlano = "Gaby Cornejo";
        //    var textoCifrado = protectorLimitadoPorTiempo.Protect(textoPlano, lifetime: TimeSpan.FromSeconds(5));
        //    Thread.Sleep(6000);
        //    var textoDesencriptado = protectorLimitadoPorTiempo.Unprotect(textoCifrado);

        //    return Ok(new
        //    {
        //        textoPlano = textoPlano,
        //        textoCifrado = textoCifrado,
        //        textoDesencriptado = textoDesencriptado
        //    });
        //}

        [HttpPost("registrar", Name = "registrarUsuario")]
        public async Task<ActionResult<RespuestaAutenticacion>> Registrar(CredencialesUsuario credencialesUsuario) 
        {
            var usuario = new IdentityUser
            {
                UserName = credencialesUsuario.Email,
                Email = credencialesUsuario.Email
            };

            var resultado = await userManager.CreateAsync(usuario, credencialesUsuario.Password);

            if(resultado.Succeeded)
            {

                return await ConstruirToken(credencialesUsuario);
            }
            else 
            {
                return BadRequest(resultado.Errors);
            }
        }

        [HttpPost("login", Name = "loginUsuario")]
        public async Task<ActionResult<RespuestaAutenticacion>> Login(CredencialesUsuario credencialesUsuario) 
        {
            var resultado = await signInManager.PasswordSignInAsync(credencialesUsuario.Email,
                                        credencialesUsuario.Password, isPersistent:false, lockoutOnFailure:false);

            if(resultado.Succeeded) 
            {
                return await ConstruirToken(credencialesUsuario);
            }
            else
            {
                return BadRequest("Login Incorrecto");
            }

        }

        [HttpGet("RenovarToken", Name = "renovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        private async Task<ActionResult<RespuestaAutenticacion>> Renovar()
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var credencialesUSuario = new CredencialesUsuario()
            {
                Email = email
            };

            return await ConstruirToken(credencialesUSuario);


        }

        private async Task<RespuestaAutenticacion> ConstruirToken(CredencialesUsuario credencialesUsuario)
        {
            //Claim: es una informacion del usuario en la cual podemos confiar
            var claims = new List<Claim>()
            {
                new Claim("email", credencialesUsuario.Email)
            };

            var usuario = await userManager.FindByEmailAsync(credencialesUsuario.Email);
            var claimDB = await userManager.GetClaimsAsync(usuario);

            claims.AddRange(claimDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddYears(1);
            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: creds);

            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiracion
            };
        }

        [HttpPost("HacerAdmin", Name = "hacerAdmin")]
        public async Task<ActionResult> HacerAdmin(EditarAdminDTO editarAdminDTO) 
        {
            var usuario = await userManager.FindByEmailAsync(editarAdminDTO.Email);
            await userManager.AddClaimAsync(usuario, new Claim("esAdmin", "1"));

            return NoContent();
        }

        [HttpPost("RemoverAdmin", Name = "removerAdmin")]
        public async Task<ActionResult> RemoverAdmin(EditarAdminDTO editarAdminDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdminDTO.Email);
            await userManager.RemoveClaimAsync(usuario, new Claim("esAdmin", "1"));

            return NoContent();
        }

    }
}
