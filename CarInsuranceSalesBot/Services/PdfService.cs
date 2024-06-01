using CarInsuranceSalesBot.Services.IServices;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarInsuranceSalesBot.Services
{
    public class PdfService : IPdfService
    {
        public byte[] GetInsuranceAgreement()
        {
            try
            {
                using (var document = new PdfDocument())
                {
                    var page = document.AddPage();
                    using (var gfx = XGraphics.FromPdfPage(page))
                    {
                        // Title
                        XFont titleFont = new XFont("Arial", 20);
                        gfx.DrawString("Insurance Policy Document", titleFont, XBrushes.Black, new XRect(0, 40, page.Width, 40), XStringFormats.TopCenter);

                        // Policy Details
                        XFont policyFont = new XFont("Arial", 12);
                        gfx.DrawString("Policy Number: INS1234567890", policyFont, XBrushes.Black, new XRect(40, 100, page.Width, 20), XStringFormats.TopLeft);
                        gfx.DrawString("Insured Name: John Doe", policyFont, XBrushes.Black, new XRect(40, 120, page.Width, 20), XStringFormats.TopLeft);
                        gfx.DrawString("Effective Date: January 1, 2024", policyFont, XBrushes.Black, new XRect(40, 140, page.Width, 20), XStringFormats.TopLeft);
                        gfx.DrawString("Expiration Date: December 31, 2024", policyFont, XBrushes.Black, new XRect(40, 160, page.Width, 20), XStringFormats.TopLeft);
                        gfx.DrawString("Policy Type: Comprehensive Auto Insurance", policyFont, XBrushes.Black, new XRect(40, 180, page.Width, 20), XStringFormats.TopLeft);
                        gfx.DrawString("Fixed Price: 100 USD", policyFont, XBrushes.Black, new XRect(40, 200, page.Width, 20), XStringFormats.TopLeft);

                        // Coverage Details
                        gfx.DrawString("Coverage Details:", policyFont, XBrushes.Black, new XRect(40, 240, page.Width, 20), XStringFormats.TopLeft);

                        XFont tableFont = new XFont("Arial", 12);
                        double tableStartY = 260;
                        double rowHeight = 20;
                        gfx.DrawString("Coverage Type", tableFont, XBrushes.Black, new XRect(40, tableStartY, 200, rowHeight), XStringFormats.TopLeft);
                        gfx.DrawString("Coverage Limit", tableFont, XBrushes.Black, new XRect(240, tableStartY, 300, rowHeight), XStringFormats.TopLeft);

                        string[,] tableData = {
                        { "Bodily Injury Liability", "$100,000 per person / $300,000 per accident" },
                        { "Property Damage Liability", "$50,000 per accident" },
                        { "Collision", "$500 deductible" },
                        { "Comprehensive", "$250 deductible" }
                    };

                        for (int i = 0; i < tableData.GetLength(0); i++)
                        {
                            gfx.DrawString(tableData[i, 0], tableFont, XBrushes.Black, new XRect(40, tableStartY + rowHeight * (i + 1), 200, rowHeight), XStringFormats.TopLeft);
                            gfx.DrawString(tableData[i, 1], tableFont, XBrushes.Black, new XRect(240, tableStartY + rowHeight * (i + 1), 300, rowHeight), XStringFormats.TopLeft);
                        }

                        // Additional Clauses
                        double additionalClausesStartY = tableStartY + rowHeight * (tableData.GetLength(0) + 2);
                        gfx.DrawString("Additional Clauses:", policyFont, XBrushes.Black, new XRect(40, additionalClausesStartY, page.Width, 20), XStringFormats.TopLeft);
                        gfx.DrawString("1. This policy covers all types of accidental damages.", policyFont, XBrushes.Black, new XRect(40, additionalClausesStartY + rowHeight, page.Width, 20), XStringFormats.TopLeft);
                        gfx.DrawString("2. The insured must report any claim within 30 days of the incident.", policyFont, XBrushes.Black, new XRect(40, additionalClausesStartY + rowHeight * 2, page.Width, 20), XStringFormats.TopLeft);
                        gfx.DrawString("3. The policy does not cover intentional damages or fraud.", policyFont, XBrushes.Black, new XRect(40, additionalClausesStartY + rowHeight * 3, page.Width, 20), XStringFormats.TopLeft);

                        // Signature Line
                        double signatureLineStartY = additionalClausesStartY + rowHeight * 5;
                        gfx.DrawString("Authorized Signature: _______________________", policyFont, XBrushes.Black, new XRect(40, signatureLineStartY, page.Width, 20), XStringFormats.TopLeft);
                    }

                    using (var stream = new MemoryStream())
                    {
                        document.Save(stream, false);
                        return stream.ToArray();
                    }
                }
            }
            catch
            {
                throw new Exception("Oops, something wrong with the insurance, try again later");
            }
        }
    }
}
