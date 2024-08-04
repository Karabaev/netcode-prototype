using Motk.Combat.Shared.Messages.Dto;
using Motk.HexGrid.Core;
using Motk.HexGrid.Core.Descriptors;

namespace Motk.Combat.Server.gRPC;

public class InitialUnitPlacementProvider
{
  public (HexCoordinates, HexDirection) GetUnitPlacement(byte teamOrder, byte unitId)
  {
    return (new HexCoordinates(), HexDirection.E);
  }
}

public class MetaStoreStub
{
  private int _requestCounter;
  
  public Task<MetaStoreUnitDto[]> GetHeroArmyAsync(string userSecret)
  {
    var descriptorId = _requestCounter % 2 == 0 ? "first" : "second";

    var units = new MetaStoreUnitDto[]
    {
      new(0, descriptorId, 1),
      new(1, descriptorId, 2),
      new(2, descriptorId, 3),
      new(3, descriptorId, 4),
      new(4, descriptorId, 5)
    };
    
    _requestCounter++;
    return Task.FromResult(units);
  }
}

public readonly struct MetaStoreUnitDto
{
  public readonly byte Id;
  public readonly string DescriptorId;
  public readonly ushort Count;
  
  public MetaStoreUnitDto(byte id, string descriptorId, ushort count)
  {
    Id = id;
    DescriptorId = descriptorId;
    Count = count;
  }
}