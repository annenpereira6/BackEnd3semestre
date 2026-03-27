using ConnectPlus.BdContextConnect;
using ConnectPlus.Interface;
using ConnectPlus.Models;
using Microsoft.EntityFrameworkCore;

namespace ConnectPlus.Repository
{
    public class ContatoRepository : IContatoRepository
    {
        private readonly ConnecttContext _context;
       
        public ContatoRepository(ConnecttContext context)
        {
            _context = context;
        }

        public List<Contato> Listar()
        {

            return _context.Contatos.ToList();
        }

        public Contato BuscarPorId(Guid id)
        {
            return _context.Contatos.Find(id)!;
        }

        public void Cadastrar(Contato contato)
        {
            _context.Contatos.Add(contato);
            _context.SaveChanges();
        }

        public void Atualizar(Guid id, Contato contato)
        {
            var contatoExistente = _context.Contatos.Find(id);

            if (contatoExistente != null)
            {
                
                contatoExistente.Nome = contato.Nome;
                contatoExistente.DadosContato = contato.DadosContato; 
                contatoExistente.Imagem = contato.Imagem;
                contatoExistente.IdTipoContato = contato.IdTipoContato;

                _context.Contatos.Update(contatoExistente);
                _context.SaveChanges();
            }
        }

        public void Deletar(Guid id)
        {
            var contatoParaRemover = _context.Contatos.Find(id);

            if (contatoParaRemover != null)
            {
                _context.Contatos.Remove(contatoParaRemover);
                _context.SaveChanges();
            }
        }
    }
}