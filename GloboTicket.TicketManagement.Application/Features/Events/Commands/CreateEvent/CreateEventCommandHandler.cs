using System.ComponentModel.DataAnnotations;
using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Infrastructure;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Models.Mail;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Guid>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;

    public CreateEventCommandHandler(IEventRepository eventRepository, IMapper mapper, IEmailService emailService)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
        _emailService = emailService;
    }
    public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var @event = _mapper.Map<Event>(request);

        var validator = new CreateEventCommandValidator(_eventRepository);
        var validationResult = await validator.ValidateAsync(request);
        if (validationResult.Errors.Count > 0)
        {
            throw new Exceptions.ValidationException(validationResult);
        }
        
        @event = await _eventRepository.AddAsync(@event);
        
        // Send an email
        var email = new Email()
        {
            To = "lewis.sneddon@icloud.com",
            Body = $"A new Event was created: {request}",
            Subject = "A new Event was created"
        };

        try
        {
            await _emailService.SendEmail(email);
        }
        catch(Exception e)
        {
            // Just log and move on
        }
        
        return @event.EventId;
    }
}