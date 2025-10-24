using System.Text.RegularExpressions;

namespace SM_LLMClient.Services
{
    public class EnhancedPiiDetector
    {
        private readonly Dictionary<string, Regex> _piiPatterns;
        private readonly Dictionary<string, string[]> _countryPhonePatterns;
        private readonly Dictionary<string, string[]> _internationalAddressPatterns;

        public EnhancedPiiDetector()
        {
            _piiPatterns = InitializePiiPatterns();
            _countryPhonePatterns = InitializeCountryPhonePatterns();
            _internationalAddressPatterns = InitializeInternationalAddressPatterns();
        }

        public PiiDetectionResult DetectPii(string input)
        {
            var detectedTypes = new List<string>();
            var confidenceScores = new Dictionary<string, double>();

            foreach (var pattern in _piiPatterns)
            {
                if (pattern.Value.IsMatch(input))
                {
                    detectedTypes.Add(pattern.Key);
                    confidenceScores[pattern.Key] = CalculateConfidence(pattern.Key, input);
                }
            }

            // Enhanced phone number detection
            var phoneResults = DetectInternationalPhoneNumbers(input);
            if (phoneResults.Any())
            {
                detectedTypes.AddRange(phoneResults.Select(r => r.Type));
                foreach (var result in phoneResults)
                {
                    confidenceScores[result.Type] = result.Confidence;
                }
            }

            // Enhanced address detection
            var addressResults = DetectInternationalAddresses(input);
            if (addressResults.Any())
            {
                detectedTypes.AddRange(addressResults.Select(r => r.Type));
                foreach (var result in addressResults)
                {
                    confidenceScores[result.Type] = result.Confidence;
                }
            }

            return new PiiDetectionResult
            {
                HasPii = detectedTypes.Any(),
                DetectedTypes = detectedTypes.Distinct().ToList(),
                ConfidenceScores = confidenceScores,
                Input = input
            };
        }

        private Dictionary<string, Regex> InitializePiiPatterns()
        {
            return new Dictionary<string, Regex>
            {
                // Enhanced SSN patterns
                ["SSN"] = new Regex(@"\b\d{3}[-]?\d{2}[-]?\d{4}\b|\b\d{9}\b", RegexOptions.Compiled),

                // Enhanced Credit Card patterns
                ["Credit Card"] = new Regex(@"\b\d{4}[\s-]?\d{4}[\s-]?\d{4}[\s-]?\d{4}\b|\b\d{4}[\s-]?\d{6}[\s-]?\d{5}\b", RegexOptions.Compiled),

                // Enhanced Email patterns
                ["Email"] = new Regex(@"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b", RegexOptions.Compiled),

                // Enhanced IP Address patterns
                ["IP Address"] = new Regex(@"\b(?:[0-9]{1,3}\.){3}[0-9]{1,3}\b", RegexOptions.Compiled),

                // Enhanced Date of Birth patterns (multiple formats)
                ["Date of Birth"] = new Regex(@"\b(?:0[1-9]|1[0-2])[/-](?:0[1-9]|[12]\d|3[01])[/-](?:19|20)\d{2}\b|\b(?:0[1-9]|[12]\d|3[01])[/-](?:0[1-9]|1[0-2])[/-](?:19|20)\d{2}\b|\b\d{4}[/-](?:0[1-9]|1[0-2])[/-](?:0[1-9]|[12]\d|3[01])\b", RegexOptions.Compiled),

                // Enhanced Passport patterns
                ["Passport Number"] = new Regex(@"\b[A-Z]{1,2}[0-9]{6,9}\b", RegexOptions.Compiled),

                // Enhanced Driver License patterns
                ["Driver License"] = new Regex(@"\b[A-Z]{1,2}[0-9]{6,8}\b", RegexOptions.Compiled),

                // Bank Account patterns
                ["Bank Account"] = new Regex(@"\b\d{8,17}\b", RegexOptions.Compiled),

                // Medical Record patterns
                ["Medical Record"] = new Regex(@"\b(?:MR|MED|HOSP)\d{6,12}\b", RegexOptions.IgnoreCase | RegexOptions.Compiled),

                // Insurance patterns
                ["Insurance"] = new Regex(@"\b(?:INS|POL|CLAIM)\d{6,12}\b", RegexOptions.IgnoreCase | RegexOptions.Compiled)
            };
        }

        private Dictionary<string, string[]> InitializeCountryPhonePatterns()
        {
            return new Dictionary<string, string[]>
            {
                ["US"] = new[] { @"\b\(?[2-9]\d{2}\)?[\s-]?\d{3}[\s-]?\d{4}\b", @"\b\+1[\s-]?[2-9]\d{2}[\s-]?\d{3}[\s-]?\d{4}\b" },
                ["Costa Rica"] = new[] { @"\b\+506[\s-]?\d{4}[\s-]?\d{4}\b", @"\b506[\s-]?\d{4}[\s-]?\d{4}\b" },
                ["UK"] = new[] { @"\b\+44[\s-]?\d{4}[\s-]?\d{6}\b", @"\b0\d{3}[\s-]?\d{6}\b" },
                ["Germany"] = new[] { @"\b\+49[\s-]?\d{3,4}[\s-]?\d{6,8}\b", @"\b0\d{3,4}[\s-]?\d{6,8}\b" },
                ["France"] = new[] { @"\b\+33[\s-]?\d{1}[\s-]?\d{2}[\s-]?\d{2}[\s-]?\d{2}[\s-]?\d{2}\b" },
                ["Japan"] = new[] { @"\b\+81[\s-]?\d{1,4}[\s-]?\d{1,4}[\s-]?\d{4}\b" },
                ["China"] = new[] { @"\b\+86[\s-]?\d{3}[\s-]?\d{4}[\s-]?\d{4}\b" },
                ["India"] = new[] { @"\b\+91[\s-]?\d{5}[\s-]?\d{5}\b", @"\b0\d{5}[\s-]?\d{5}\b" },
                ["Brazil"] = new[] { @"\b\+55[\s-]?\d{2}[\s-]?\d{4,5}[\s-]?\d{4}\b" },
                ["Canada"] = new[] { @"\b\+1[\s-]?[2-9]\d{2}[\s-]?\d{3}[\s-]?\d{4}\b" },
                ["Australia"] = new[] { @"\b\+61[\s-]?\d{1}[\s-]?\d{4}[\s-]?\d{4}\b" },
                ["Mexico"] = new[] { @"\b\+52[\s-]?\d{2,3}[\s-]?\d{3}[\s-]?\d{4}\b" }
            };
        }

        private Dictionary<string, string[]> InitializeInternationalAddressPatterns()
        {
            return new Dictionary<string, string[]>
            {
                ["US"] = new[] { 
                    // US addresses with state and ZIP code
                    @"\b\d+\s+[A-Za-z\s]+(?:Street|St|Avenue|Ave|Road|Rd|Boulevard|Blvd|Lane|Ln|Drive|Dr|Way|Court|Ct|Place|Pl|Circle|Cir|Highway|Hwy|Parkway|Pkwy)\s*,\s*[A-Za-z\s]+,\s*[A-Z]{2}\s+\d{5}(?:-\d{4})?\b",
                    // US addresses with just street and city
                    @"\b\d+\s+[A-Za-z\s]+(?:Street|St|Avenue|Ave|Road|Rd|Boulevard|Blvd|Lane|Ln|Drive|Dr|Way|Court|Ct|Place|Pl|Circle|Cir|Highway|Hwy|Parkway|Pkwy)\s*,\s*[A-Za-z\s]+,\s*(?:AL|AK|AZ|AR|CA|CO|CT|DE|FL|GA|HI|ID|IL|IN|IA|KS|KY|LA|ME|MD|MA|MI|MN|MS|MO|MT|NE|NV|NH|NJ|NM|NY|NC|ND|OH|OK|OR|PA|RI|SC|SD|TN|TX|UT|VT|VA|WA|WV|WI|WY)\b"
                },
                ["UK"] = new[] { 
                    // UK addresses with postcode
                    @"\b\d+\s+[A-Za-z\s]+(?:Street|St|Road|Rd|Lane|Ln|Avenue|Ave|Close|Cl|Crescent|Cres|Drive|Dr|Way|Place|Pl)\s*,\s*[A-Za-z\s]+,\s*[A-Z]{1,2}\d{1,2}\s+\d[A-Z]{2}\b",
                    // UK addresses with London postcode
                    @"\b\d+\s+[A-Za-z\s]+(?:Street|St|Road|Rd|Lane|Ln|Avenue|Ave|Close|Cl|Crescent|Cres|Drive|Dr|Way|Place|Pl)\s*,\s*London\s*,\s*[A-Z]{1,2}\d{1,2}\s+\d[A-Z]{2}\b"
                },
                ["Germany"] = new[] { 
                    // German addresses with postal code
                    @"\b[A-Za-z\s]+\s+\d+[a-z]?\s*,\s*\d{5}\s+[A-Za-z\s]+\b",
                    // German addresses with specific German cities
                    @"\b[A-Za-z\s]+\s+\d+[a-z]?\s*,\s*\d{5}\s+(?:Berlin|Munich|Hamburg|Frankfurt|Cologne|Stuttgart|Düsseldorf|Dortmund|Essen|Leipzig)\b"
                },
                ["France"] = new[] { 
                    // French addresses with postal code
                    @"\b\d+\s+[A-Za-z\s]+(?:Rue|Avenue|Boulevard|Place|Square|Impasse|Chemin|Route)\s*,\s*\d{5}\s+[A-Za-z\s]+\b",
                    // French addresses with Paris
                    @"\b\d+\s+[A-Za-z\s]+(?:Rue|Avenue|Boulevard|Place|Square|Impasse|Chemin|Route)\s*,\s*\d{5}\s+Paris\b"
                },
                ["Japan"] = new[] { 
                    // Japanese addresses with postal code
                    @"\b\d{3}-\d{4}\s+[A-Za-z\s]+\b",
                    // Japanese addresses with Tokyo
                    @"\b[A-Za-z\s]+\s+\d+-\d+\s*,\s*Tokyo\b"
                },
                ["Canada"] = new[] { 
                    // Canadian addresses with postal code
                    @"\b\d+\s+[A-Za-z\s]+(?:Street|St|Avenue|Ave|Road|Rd|Boulevard|Blvd|Lane|Ln|Drive|Dr|Way|Court|Ct|Place|Pl|Circle|Cir)\s*,\s*[A-Za-z\s]+,\s*[A-Z]{2}\s+\d[A-Z]\d\s+\d[A-Z]\d\b",
                    // Canadian addresses with provinces
                    @"\b\d+\s+[A-Za-z\s]+(?:Street|St|Avenue|Ave|Road|Rd|Boulevard|Blvd|Lane|Ln|Drive|Dr|Way|Court|Ct|Place|Pl|Circle|Cir)\s*,\s*[A-Za-z\s]+,\s*(?:ON|QC|BC|AB|MB|SK|NS|NB|NL|PE|YT|NT|NU)\b"
                },
                ["India"] = new[] {
                    // Indian addresses with PIN code
                    @"\b(?:Flat|House|Plot|Block|Sector|Area|Colony|Apartment|Building|Complex)\s*[A-Za-z0-9\s]+\s*,\s*[A-Za-z\s]+\s*,\s*[A-Za-z\s]+\s*,\s*\d{6}\s+India\b",
                    // Indian addresses with state and PIN
                    @"\b(?:Flat|House|Plot|Block|Sector|Area|Colony|Apartment|Building|Complex)\s*[A-Za-z0-9\s]+\s*,\s*[A-Za-z\s]+\s*,\s*(?:Karnataka|Tamil Nadu|Maharashtra|Gujarat|Rajasthan|Uttar Pradesh|West Bengal|Andhra Pradesh|Telangana|Kerala|Punjab|Haryana|Himachal Pradesh|Jammu and Kashmir|Uttarakhand|Bihar|Jharkhand|Odisha|Chhattisgarh|Madhya Pradesh|Assam|Manipur|Meghalaya|Mizoram|Nagaland|Tripura|Arunachal Pradesh|Sikkim|Goa|Delhi|Chandigarh|Puducherry|Andaman and Nicobar Islands|Lakshadweep|Dadra and Nagar Haveli|Daman and Diu)\s*,\s*\d{6}\b",
                    // Indian addresses with major cities
                    @"\b(?:Flat|House|Plot|Block|Sector|Area|Colony|Apartment|Building|Complex)\s*[A-Za-z0-9\s]+\s*,\s*(?:Mumbai|Delhi|Bangalore|Hyderabad|Chennai|Kolkata|Pune|Ahmedabad|Jaipur|Surat|Lucknow|Kanpur|Nagpur|Indore|Thane|Bhopal|Visakhapatnam|Pimpri|Patna|Vadodara|Ghaziabad|Ludhiana|Agra|Nashik|Faridabad|Meerut|Rajkot|Kalyan|Vasai|Varanasi|Srinagar|Aurangabad|Navi Mumbai|Solapur|Vijayawada|Kolhapur|Amritsar|Noida|Ranchi|Howrah|Coimbatore|Raipur|Gwalior|Chandigarh|Tiruchirappalli|Mysore|Bhubaneswar|Kochi|Bhavnagar|Salem|Warangal|Guntur|Bhiwandi|Amravati|Nanded|Kolhapur|Sangli|Malegaon|Ulhasnagar|Jalgaon|Akola|Latur|Ahmadnagar|Dhule|Ichalkaranji|Parbhani|Jalna|Bhusawal|Panvel|Satara|Beed|Yavatmal|Kamptee|Gondia|Barshi|Achalpur|Osmanabad|Nandurbar|Wardha|Udgir|Hinganghat)\s*,\s*\d{6}\b"
                }
            };
        }

        private List<PhoneDetectionResult> DetectInternationalPhoneNumbers(string input)
        {
            var results = new List<PhoneDetectionResult>();

            foreach (var country in _countryPhonePatterns)
            {
                foreach (var pattern in country.Value)
                {
                    var regex = new Regex(pattern, RegexOptions.Compiled);
                    if (regex.IsMatch(input))
                    {
                        results.Add(new PhoneDetectionResult
                        {
                            Type = $"Phone Number ({country.Key})",
                            Country = country.Key,
                            Confidence = CalculatePhoneConfidence(pattern, input)
                        });
                    }
                }
            }

            return results;
        }

        private List<AddressDetectionResult> DetectInternationalAddresses(string input)
        {
            var results = new List<AddressDetectionResult>();

            foreach (var country in _internationalAddressPatterns)
            {
                foreach (var pattern in country.Value)
                {
                    var regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    var matches = regex.Matches(input);

                    foreach (Match match in matches)
                    {
                        var confidence = CalculateAddressConfidence(pattern, input);

                        // Only add if confidence is above threshold to reduce false positives
                        if (confidence > 0.7)
                        {
                            results.Add(new AddressDetectionResult
                            {
                                Type = $"Address ({country.Key})",
                                Country = country.Key,
                                Confidence = confidence,
                                Value = match.Value
                            });
                        }
                    }
                }
            }

            return results;
        }

        private double CalculateConfidence(string piiType, string input)
        {
            return piiType switch
            {
                "SSN" => 0.95,
                "Credit Card" => 0.90,
                "Email" => 0.85,
                "Phone Number" => 0.80,
                "Address" => 0.75,
                "IP Address" => 0.90,
                "Date of Birth" => 0.70,
                "Passport Number" => 0.85,
                "Driver License" => 0.80,
                _ => 0.70
            };
        }

        private double CalculatePhoneConfidence(string pattern, string input)
        {
            var regex = new Regex(pattern, RegexOptions.Compiled);
            var match = regex.Match(input);

            if (!match.Success) return 0.0;

            var phoneNumber = match.Value.Replace("+", "").Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");

            // Higher confidence for longer numbers and specific patterns
            if (phoneNumber.Length >= 10) return 0.90;
            if (phoneNumber.Length >= 8) return 0.80;
            return 0.70;
        }

        private double CalculateAddressConfidence(string pattern, string input)
        {
            var regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var match = regex.Match(input);

            if (!match.Success) return 0.0;

            var address = match.Value.ToLowerInvariant();

            // Higher confidence for addresses with postal codes and country indicators
            if (address.Contains("india") || address.Contains("karnataka") || address.Contains("bengaluru"))
                return 0.95; // High confidence for Indian addresses

            if (address.Contains("usa") || address.Contains("united states") || address.Contains("california") || address.Contains("new york"))
                return 0.90; // High confidence for US addresses

            if (address.Contains("london") || address.Contains("uk") || address.Contains("united kingdom"))
                return 0.90; // High confidence for UK addresses

            if (address.Contains("germany") || address.Contains("berlin") || address.Contains("munich"))
                return 0.90; // High confidence for German addresses

            if (address.Contains("canada") || address.Contains("toronto") || address.Contains("vancouver"))
                return 0.90; // High confidence for Canadian addresses

            // Lower confidence for generic patterns
            if (address.Contains("flat") || address.Contains("block") || address.Contains("apartment"))
                return 0.60; // Lower confidence for generic building terms

            // Higher confidence for addresses with postal codes
            if (address.Contains(",") && (address.Contains(" ") || address.Contains("-"))) return 0.85;
            if (address.Contains(",")) return 0.75;
            return 0.65;
        }
    }

    public class PiiDetectionResult
    {
        public bool HasPii { get; set; }
        public List<string> DetectedTypes { get; set; } = new();
        public Dictionary<string, double> ConfidenceScores { get; set; } = new();
        public string Input { get; set; } = string.Empty;
    }

    public class PhoneDetectionResult
    {
        public string Type { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public double Confidence { get; set; }
    }

    public class AddressDetectionResult
    {
        public string Type { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public double Confidence { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}
