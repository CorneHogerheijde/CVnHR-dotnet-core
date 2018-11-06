using System;
using System.Collections.Generic;
using System.Text;

namespace CVnHR.Business.Services
{
    public interface ISettingsService
    {
        T GetSettings<T>();

        void UpdateSettings<T>(T newSettings);
    }
}
