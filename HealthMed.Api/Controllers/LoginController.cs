using HealthMed.Api.Dtos;
using HealthMed.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Api.Controllers;

[ApiController]
[Route("api/login")]
public class LoginController : ControllerBase
{
    private readonly IAuthService _authService;

    public LoginController(IAuthService authService)
    {
        _authService = authService;
    }
    /// <summary>
    /// Realiza o login de um médico.
    /// </summary>
    /// <param name="model">Dados de login do médico.</param>
    /// <returns>Token de autenticação.</returns>
    /// <response code="200">Login realizado com sucesso!</response>
    /// <response code="400">CRM e Senha são obrigatórios :(</response>
    /// <response code="401">CRM ou senha inválidos :(</response>
    [HttpPost("medico")]
    public async Task<IActionResult> LoginMedico([FromBody] LoginMedicoDto model)
    {
        if (string.IsNullOrEmpty(model.CRM) || string.IsNullOrEmpty(model.Senha))
            return BadRequest("CRM e Senha são obrigatórios.");

        var token = await _authService.AutenticarMedico(model.CRM, model.Senha);

        if (token == null)
            return Unauthorized("CRM ou senha inválidos.");

        return Ok(new { Token = token });
    }

    /// <summary>
    /// Realiza o login de um paciente.
    /// </summary>
    /// <param name="model">Dados de login do paciente.</param>
    /// <returns>Token de autenticação.</returns>
    /// <response code="200">Login realizado com sucesso!</response>
    /// <response code="400">CPF e Senha são obrigatórios :(</response>
    /// <response code="401">CPF ou senha inválidos :(</response>
    [HttpPost("paciente")]
    public async Task<IActionResult> LoginPaciente([FromBody] LoginPacienteDto model)
    {
        if (string.IsNullOrEmpty(model.CPF) || string.IsNullOrEmpty(model.Senha))
            return BadRequest("CPF e Senha são obrigatórios.");

        var token = await _authService.AutenticarPaciente(model.CPF, model.Senha);

        if (token == null)
            return Unauthorized("CPF ou senha inválidos.");

        return Ok(new { Token = token });
    }
}
