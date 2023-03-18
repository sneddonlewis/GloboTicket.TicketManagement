using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryCommandResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }
    
    public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateCategoryCommandValidator();
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Count > 0)
        {
            var errorResponse = new CreateCategoryCommandResponse();
            errorResponse.Success = false;
            errorResponse.ValidationErrors = new List<string>();
            foreach (var error in validationResult.Errors)
            {
                errorResponse.ValidationErrors.Add(error.ErrorMessage);
            }
            return errorResponse;
        }

        var response = new CreateCategoryCommandResponse();
        var category = new Category() { Name = request.Name };
        category = await _categoryRepository.AddAsync(category);
        response.Category = _mapper.Map<CreateCategoryDto>(category);
        return response;
    }
}