using System;
using Fohjin.DDD.Commands;
using Fohjin.DDD.Contracts;
using Fohjin.DDD.Domain.Entities;
using Fohjin.DDD.Domain.ValueObjects;

namespace Fohjin.DDD.CommandHandlers
{
    public class ClientChangedTheirNameCommandHandler : ICommandHandler<ClientChangedTheirNameCommand>
    {
        private readonly IDomainRepository _repository;

        public ClientChangedTheirNameCommandHandler(IDomainRepository repository)
        {
            _repository = repository;
        }

        public void Execute(ClientChangedTheirNameCommand command)
        {
            var client = _repository.GetById<Client>(command.Id);

            client.UpdateClientName(new ClientName(command.ClientName));

            _repository.Save(client);
        }
    }
}