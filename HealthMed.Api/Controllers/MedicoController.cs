using HealthMed.Api.Dtos;
using HealthMed.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthMed.Api.Controllers;

[ApiController]
[Route("api/medicos")]
public class MedicoController : ControllerBase
{
    private readonly IMedicoService _medicoService;
    private int _userId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    public MedicoController(IMedicoService medicoService)
    {
        _medicoService = medicoService;
    }

    /// <summary>
    /// Cadastra um novo médico.
    /// </summary>
    /// <param name="model">Dados de cadastro do médico.</param>
    /// <returns>Confirmação de cadastro.</returns>
    /// <response code="200">Médico cadastrado com sucesso!</response>
    /// <response code="400">E-mail já cadastrado :(</response>
    [AllowAnonymous]
    [HttpPost("cadastrar")]
    public async Task<IActionResult> CadastrarMedico([FromBody] CadastroMedicoDto model)
    {
        var sucesso = await _medicoService.Cadastrar(model);
        if (!sucesso) return BadRequest("E-mail já cadastrado.");
        return Ok("Médico cadastrado com sucesso.");
    }

    /// <summary>
    /// Obtém o perfil do médico logado.
    /// </summary>
    /// <returns>Dados do perfil do médico.</returns>
    /// <response code="200">Perfil do médico retornado com sucesso!</response>
    /// <response code="404">Médico não encontrado :(</response>
    [Authorize(Roles = "Medico")]
    [HttpGet("perfil")]
    public async Task<IActionResult> GetPerfil()
    {
        var medico = await _medicoService.ObterPorUsuarioId(_userId);
        if (medico == null) return NotFound("Médico não encontrado.");
        return Ok(medico);
    }

    /// <summary>
    /// Atualiza os dados do médico logado.
    /// </summary>
    /// <param name="model">Dados de atualização do médico.</param>
    /// <returns>Confirmação de atualização.</returns>
    /// <response code="200">Dados atualizados com sucesso!</response>
    /// <response code="400">Erro ao atualizar :(</response>
    [Authorize(Roles = "Medico")]
    [HttpPut("atualizar")]
    public async Task<IActionResult> AtualizarMedico([FromBody] AtualizarMedicoDto model)
    {
        var atualizado = await _medicoService.Atualizar(_userId, model);
        if (!atualizado) return BadRequest("Erro ao atualizar.");
        return Ok("Dados atualizados com sucesso.");
    }

    /// <summary>
    /// Deleta a conta do médico logado.
    /// </summary>
    /// <returns>Confirmação de exclusão.</returns>
    /// <response code="200">Médico deletado com sucesso!</response>
    /// <response code="400">Erro ao deletar conta :(</response>
    [Authorize(Roles = "Medico")]
    [HttpDelete("deletar")]
    public async Task<IActionResult> DeletarMedico()
    {
        var deletado = await _medicoService.Deletar(_userId);
        if (!deletado) return BadRequest("Erro ao deletar conta.");
        return Ok("Médico deletado com sucesso.");
    }
}
