using Domain.Models;
using System.Threading.Tasks;

namespace Domain
{
    public interface ICloudScaleService
    {
        void SendTranslateTasks(TranslateRequest tasks);
    }
}