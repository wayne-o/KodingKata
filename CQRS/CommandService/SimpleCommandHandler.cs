// -----------------------------------------------------------------------
// <copyright file="SimpleCommandHandler.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace CommandService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Domain;

    using Magnum;

    using MassTransit;
    using MassTransit.Util;

    using Messages;

    using NLog;

    using Sonatribe.Domain;

    public class SimpleCommandHandler : Consumes<CreateNewUser>.Context
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly Func<IDomainRepository> _repository;

        public SimpleCommandHandler([NotNull] Func<IDomainRepository> repository)
        {
            _repository = repository;
        }

        public void Consume(IConsumeContext<CreateNewUser> command)
        {
            _logger.Info(command.Message.UserName);

            var c = command.Message;

            try
            {
                var repo = _repository();

                var user = User.CreateNew(c.AggregateId, new UserName(c.UserName), new EmailAddress(c.Email), c.Password, c.ActivationKey, c.SendActivationEmail);

                repo.Save(user, CombGuid.Generate(), null);
            }
            catch (Exception exc)
            {
                _logger.Error(exc.Message);
                _logger.Error(exc.StackTrace);
            }
        }
    }
}
