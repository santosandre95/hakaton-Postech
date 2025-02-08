using HealthMed.Api.Dtos;
using HealthMed.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AgendamentoController : ControllerBase
{
    private readonly IAgendamentoService _agendamentoService;

    public AgendamentoController(IAgendamentoService agendamentoService)
    {
        _agendamentoService = agendamentoService;
    }

    /// <summary>
    /// Lista todos os médicos.
    /// </summary>
    /// <returns>Uma lista de médicos.</returns>
    /// <response code="200">Lista de médicos retornada com sucesso!</response>
    /// <response code="404">Nenhum médico encontrado :(</response>
    [Authorize(Roles = "Paciente")]
    [HttpGet("medicos")]
    public async Task<IActionResult> ListarMedicos()
    {
        var medicos = await _agendamentoService.ListarMedicos();
        return Ok(medicos);
    }

    /// <summary>
    /// Lista todos os agendamentos de um médico.
    /// </summary>
    /// <param name="medicoId">Id do médico.</param>
    /// <param name="status">Status dos agendamentos.</param>
    /// <returns>Uma lista de agendamentos.</returns>
    /// <response code="200">Lista de agendamentos retornada com sucesso!</response>
    /// <response code="404">Nenhum agendamento encontrado :(</response>
    [Authorize(Roles = "Medico")]
    [HttpGet("{medicoId}/agendamentos")]
    public async Task<IActionResult> ListarAgendamentos(int medicoId, [FromQuery] StatusAgendamento status = StatusAgendamento.Todos)
    {
        var agendamentos = await _agendamentoService.Listar(medicoId, status);
        return Ok(agendamentos);
    }

    /// <summary>
    /// Obtém horários disponíveis de um médico.
    /// </summary>
    /// <param name="medicoId">Id do médico.</param>
    /// <param name="data">Data desejada.</param>
    /// <returns>Uma lista de horários disponíveis.</returns>
    /// <response code="200">Horários disponíveis retornados com sucesso!</response>
    /// <response code="404">Nenhum horário disponível encontrado :(</response>
    [Authorize(Roles = "Paciente")]
    [HttpGet("{medicoId}/horarios-disponiveis")]
    public async Task<IActionResult> ObterHorariosDisponiveis(int medicoId, [FromQuery] DateTime data)
    {
        var horarios = await _agendamentoService.ObterHorariosDisponiveis(medicoId, data);
        return Ok(horarios);
    }

    /// <summary>
    /// Cria um novo agendamento.
    /// </summary>
    /// <param name="dto">Dados do agendamento.</param>
    /// <returns>Confirmação de agendamento.</returns>
    /// <response code="200">Agendamento realizado com sucesso!</response>
    /// <response code="400">Erro ao realizar o agendamento :(</response>
    [Authorize(Roles = "Paciente")]
    [HttpPost]
    public async Task<IActionResult> CriarAgendamento([FromBody] CadastroAgendamentoDto dto)
    {
        await _agendamentoService.Criar(dto);
        return Ok("Agendamento realizado com sucesso.");
    }

    /// <summary>
    /// Aceita um agendamento.
    /// </summary>
    /// <param name="id">Id do agendamento.</param>
    /// <returns>Confirmação de aceitação.</returns>
    /// <response code="200">Agendamento aceito com sucesso!</response>
    /// <response code="400">Erro ao aceitar o agendamento :(</response>
    [Authorize(Roles = "Medico")]
    [HttpPut("{id}/aceitar")]
    public async Task<IActionResult> Aceitar(int id)
    {
        await _agendamentoService.Aceitar(id);
        return Ok("Agendamento aceito.");
    }

    /// <summary>
    /// Rejeita um agendamento.
    /// </summary>
    /// <param name="id">Id do agendamento.</param>
    /// <returns>Confirmação de rejeição.</returns>
    /// <response code="200">Agendamento rejeitado com sucesso!</response>
    /// <response code="400">Erro ao rejeitar o agendamento :(</response>
    [Authorize(Roles = "Medico")]
    [HttpPut("{id}/rejeitar")]
    public async Task<IActionResult> Rejeitar(int id)
    {
        await _agendamentoService.Rejeitar(id);
        return Ok("Agendamento rejeitado.");
    }

    /// <summary>
    /// Cancela um agendamento.
    /// </summary>
    /// <param name="id">Id do agendamento.</param>
    /// <param name="motivoCancelamento">Motivo do cancelamento.</param>
    /// <returns>Confirmação de cancelamento.</returns>
    /// <response code="200">Agendamento cancelado com sucesso!</response>
    /// <response code="400">Erro ao cancelar o agendamento :(</response>
    [Authorize(Roles = "Paciente")]
    [HttpPut("{id}/cancelar")]
    public async Task<IActionResult> Cancelar(int id, string motivoCancelamento)
    {
        await _agendamentoService.Cancelar(id, motivoCancelamento);
        return Ok("Agendamento cancelado.");
    }
}
