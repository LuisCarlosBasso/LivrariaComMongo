using System;
using LivrariaComMongo.Domain.Commands.Input;
using LivrariaComMongo.Domain.Commands.Output;
using LivrariaComMongo.Domain.Entidades;
using LivrariaComMongo.Domain.Interfaces.Repositories;
using LivrariaComMongo.Infra.Interfaces.Commands;

namespace LivrariaComMongo.Domain.Handlers
{
    public class LivroHandler : ICommandHandler<AdicionarLivroCommand>, ICommandHandler<AtualizarLivroCommand>,
        ICommandHandler<ExcluirLivroCommand>
    {
        private readonly ILivroRepository _repository;

        public LivroHandler(ILivroRepository repository)
        {
            _repository = repository;
        }

        public ICommandResult Handle(AdicionarLivroCommand command)
        {
            if (!command.ValidarCommand())
                return new LivroCommandResult(false, "Por favor corrija as inconsistências abaixo",
                    command.Notifications);

            Livro livro = new Livro(command.Nome, command.Autor, command.Edicao, command.Isbn, command.Imagem);

            _repository.Inserir(livro);

            var retorno = new LivroCommandResult(true, "Livro adicionado com sucesso!", livro);

            return retorno;
        }

        public ICommandResult Handle(AtualizarLivroCommand command)
        {
            if (!command.ValidarCommand())
                return new LivroCommandResult(false, "Por favor corrija as inconsistências abaixo",
                    command.Notifications);

            if (!_repository.CheckId(command.Id))
                return new LivroCommandResult(false, "Este livro não existe.", new { });

            string id = command.Id;
            
            Livro livro =
                new Livro(id, command.Nome, command.Autor, command.Edicao, command.Isbn, command.Imagem);
            _repository.Atualizar(livro);
            return new LivroCommandResult(true, "Livro atualizado com sucesso", livro);
        }

        public ICommandResult Handle(ExcluirLivroCommand command)
        {
            if (!command.ValidarCommand())
                return new LivroCommandResult(false, "Por favor corrija as inconsistências abaixo",
                    command.Notifications);

            if (!_repository.CheckId(command.Id))
                return new LivroCommandResult(false, "Este livro não existe.", new { });

            string id = command.Id;
            _repository.Excluir(id);
            return new LivroCommandResult(true, "Livro excluido com sucesso", new { });
        }
    }
}