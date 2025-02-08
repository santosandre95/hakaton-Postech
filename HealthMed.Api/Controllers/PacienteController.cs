using HealthMed.Api.Dtos;
using HealthMed.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthMed.Api.Controllers;

[ApiController]
[Route("api/pacientes")]
public class PacienteController : ControllerBase
{
    private readonly IPacienteService _pacienteService;
    private int _userId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    public PacienteController(IPacienteService pacienteService)
    {
        _pacienteService = pacienteService;
    }

    /// <summary>
    /// Cadastra um novo paciente.
    /// </summary>
    /// <param name="model">Dados de cadastro do paciente.</param>
    /// <returns>Confirmação de cadastro.</returns>
    /// <response code="200">Paciente cadastrado com sucesso!</response>
    /// <response code="400">E-mail já cadastrado :(</response>
    [AllowAnonymous]
    [HttpPost("cadastrar")]
    public async Task<IActionResult> CadastrarPaciente([FromBody] CadastroPacienteDto model)
    {
        var sucesso = await _pacienteService.Cadastrar(model);
        if (!sucesso) return BadRequest("E-mail já cadastrado.");
        return Ok("Paciente cadastrado com sucesso.");
    }

    /// <summary>
    /// Obtém o perfil do paciente logado.
    /// </summary>
    /// <returns>Dados do perfil do paciente.</returns>
    /// <response code="200">Perfil do paciente retornado com sucesso!</response>
    /// <response code="404">Paciente não encontrado :(</response>
    [Authorize(Roles = "Paciente")]
    [HttpGet("perfil")]
    public async Task<IActionResult> GetPerfil()
    {
        var paciente = await _pacienteService.ObterPorUsuarioId(_userId);
        if (paciente == null) return NotFound("Paciente não encontrado.");
        return Ok(paciente);
    }

    /// <summary>
    /// Atualiza os dados do paciente logado.
    /// </summary>
    /// <param name="model">Dados de atualização do paciente.</param>
    /// <returns>Confirmação de atualização.</returns>
    /// <response code="200">Dados atualizados com sucesso!</response>
    /// <response code="400">Erro ao atualizar :(</response>
    [Authorize(Roles = "Paciente")]
    [HttpPut("atualizar")]
    public async Task<IActionResult> AtualizarPaciente([FromBody] AtualizarPacienteDto model)
    {
        var atualizado = await _pacienteService.Atualizar(_userId, model);
        if (!atualizado) return BadRequest("Erro ao atualizar.");
        return Ok("Dados atualizados com sucesso.");
    }

    /// <summary>
    /// Deleta a conta do paciente logado.
    /// </summary>
    /// <returns>Confirmação de exclusão.</returns>
    /// <response code="200">Paciente deletado com sucesso!</response>
    /// <response code="400">Erro ao deletar conta :(</response>
    [Authorize(Roles = "Paciente")]
    [HttpDelete("deletar")]
    public async Task<IActionResult> DeletarPaciente()
    {
        var deletado = await _pacienteService.Deletar(_userId);
        if (!deletado) return BadRequest("Erro ao deletar conta.");
        return Ok("Paciente deletado com sucesso.");
    }
}
