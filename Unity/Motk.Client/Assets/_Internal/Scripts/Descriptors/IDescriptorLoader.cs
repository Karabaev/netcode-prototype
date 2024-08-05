using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Motk.Descriptors
{
  public interface IDescriptorLoader
  {
    Task LoadAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<DescriptorData>> GetDescriptorsAsync(string category, string type, CancellationToken ct);
  }
}