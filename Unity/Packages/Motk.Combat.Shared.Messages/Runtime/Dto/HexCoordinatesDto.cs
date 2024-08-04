using MessagePack;

namespace Motk.Combat.Shared.Messages.Dto
{
  [MessagePackObject]
  public readonly struct HexCoordinatesDto
  {
    [Key(0)] public readonly int Q;
    [Key(1)] public readonly int R;

    public HexCoordinatesDto(int q, int r)
    {
      Q = q;
      R = r;
    }
  }
}