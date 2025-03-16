using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Facial.AgeAndGender.Interface;

public interface IGenderAnalyzer
{
    Task<string> AnalyzeGenderFromUrlAsync(string imageUrl);
    Task<string> AnalyzeGenderFromBase64Async(string base64Image);
    Task<string> AnalyzeGenderFromStreamAsync(Stream fileStream);
}
