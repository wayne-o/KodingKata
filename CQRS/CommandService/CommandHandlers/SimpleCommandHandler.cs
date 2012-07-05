// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleCommandHandler.cs" company="Project Attack Ltd">
//   2012 Project Attack Ltd
// </copyright>
// <summary>
//   TODO: Update summary.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CommandService.CommandHandlers
{
    using MassTransit;

    using Messages;

    using NLog;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SimpleCommandHandler : Consumes<SimpleCommand>.Context
    {
        #region Constants and Fields

        /// <summary>
        /// The _logger.
        /// </summary>
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The consume.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Consume(IConsumeContext<SimpleCommand> message)
        {
            _logger.Info(message.Message.Message);
        }

        #endregion
    }
}