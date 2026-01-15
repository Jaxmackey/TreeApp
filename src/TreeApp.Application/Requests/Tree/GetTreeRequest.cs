using MediatR;
using TreeApp.Application.DTOs;

namespace TreeApp.Application.Requests.Tree;

public record GetTreeRequest(string TreeName) : IRequest<MNode>;