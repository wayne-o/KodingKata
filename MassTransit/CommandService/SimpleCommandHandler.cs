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

    using MassTransit;

    using Messages;

    using NLog;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SimpleCommandHandler : Consumes<SimpleCommand>.Context
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Consume(IConsumeContext<SimpleCommand> message)
        {
            _logger.Info(message.Message.Message);
        }
    }
}
