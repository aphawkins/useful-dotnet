namespace Useful.Security.Cryptography
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using TechTalk.SpecFlow;

    [Binding]
    [CLSCompliant(false)]
    public class MonoAlphabeticSpecFlowSteps
    {
        [Given(@"I am testing Monoalphabetic:")]
        public void GivenIAmTesting(Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                CipherTransformMode mode = string.Equals(row["TransformMode"], "Encrypt", StringComparison.Ordinal) ? CipherTransformMode.Encrypt : CipherTransformMode.Decrypt;
                string input = mode == CipherTransformMode.Encrypt ? row["Plaintext"] : row["Ciphertext"];
                string output = mode == CipherTransformMode.Encrypt ? row["Ciphertext"] : row["Plaintext"];
                MonoAlphabetic target = new MonoAlphabetic();
                CipherTestUtilities.TestTarget(target, mode, row["Key"], row["IV"], input, output, row["NewIV"]);
            }
        }
    }
}
