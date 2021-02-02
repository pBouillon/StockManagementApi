using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commons.Behaviors
{
    /// <summary>
    /// Pipeline step during which the request will be validated against the defined <see cref="IValidator"/>
    /// </summary>
    /// <typeparam name="TRequest">Incoming request</typeparam>
    /// <typeparam name="TResponse">Response produced by the request</typeparam>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// <see cref="IValidator"/> to by applied on the incoming request
        /// </summary>
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        /// Pipeline step constructor
        /// </summary>
        /// <param name="validators"><see cref="IValidator"/> to by applied on the incoming request</param>
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
            => _validators = validators;

        /// <inheritdoc />
        public async Task<TResponse> Handle(
            TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults.SelectMany(r => r.Errors)
                .Where(validationFailure => validationFailure != null)
                .ToList();

            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}
