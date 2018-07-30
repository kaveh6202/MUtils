using System;

namespace MUtils.Configuration
{
    public interface IConfigurationService<out T> where T : BaseConfigurationModel
    {
        T Get();
    }
}
