using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarInsuranceSalesBot.Services.IServices
{
    public interface IPdfService
    {
        byte[] GetInsuranceAgreement();
    }
}
