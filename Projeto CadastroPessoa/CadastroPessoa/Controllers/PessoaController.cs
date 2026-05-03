using CadastroPessoa.Services;
using Microsoft.AspNetCore.Mvc;

namespace CadastroPessoa.Controllers;

[ApiController]
[Route("pessoa")]
public class PessoaController : ControllerBase {

    [HttpPost("criar")]
    public ActionResult Criar([FromBody] Pessoa pessoa) {
        if (pessoa == null) return BadRequest("Dados inválidos");

        var rabbit = new CriaMensagemRabbitMQ();

        rabbit.CriaMensagem(pessoa.Nome);

        return Ok(pessoa);
    }

}

public class Pessoa {
    public string Nome { get; set; }
    public int Idade { get; set; }
}
