using System;

namespace Application.Features.Products.Commands.DecreaseStock;

public record DecreaseProductStockCommand(List<(Guid Id, int StockToBeDecreased)> Products) : ICommand;
