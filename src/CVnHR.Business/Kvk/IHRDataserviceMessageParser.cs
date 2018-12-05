using System;
using System.Collections.Generic;
using System.Text;

namespace CVnHR.Business.Kvk
{
    public interface IHRDataserviceMessageParser
    {
        MaatschappelijkeActiviteitResponseType Parse(string message);
    }
}
