using Calculator;

namespace Tests
{
    public class Tests
    {
        [Test]
        public void TestTokenizer()
        {
            var testString = "20333 + 1 * 1.11111111";
            var t = new Tokenizer(new StringReader(testString));
            Assert.Multiple(() =>
            {

                // "20333"
                Assert.That(t.Token, Is.EqualTo(Token.Number));
                Assert.That(t.Number, Is.EqualTo(20333));
            });
            t.NextToken();

            // "+"
            Assert.That(t.Token, Is.EqualTo(Token.Add));
            t.NextToken();
            Assert.Multiple(() =>
            {

                // "1"
                Assert.That(t.Token, Is.EqualTo(Token.Number));
                Assert.That(t.Number, Is.EqualTo(1));
            });
            t.NextToken();

            // "*"
            Assert.That(t.Token, Is.EqualTo(Token.Multiply));
            t.NextToken();
            Assert.Multiple(() =>
            {
                // "1.11111111"
                Assert.That(t.Token, Is.EqualTo(Token.Number));
                Assert.That(t.Number, Is.EqualTo(1.11111111m));
            });
            t.NextToken();
        }

        [Test]
        public void AddSubtractTest()
        {
            Assert.Multiple(() =>
            {
                Assert.That(Parser.Parse("10.111111111111111 + 20").Eval(), Is.EqualTo(30.111111111111111m));
                Assert.That(Parser.Parse("10.1 - 20.11").Eval(), Is.EqualTo(-10.01m));
                Assert.That(Parser.Parse("10 + 20 - 40 + 100").Eval(), Is.EqualTo(90));
            });
        }

        [Test]
        public void TestUnary()
        {
            Assert.Multiple(() =>
            {
                Assert.That(Parser.Parse("-10").Eval(), Is.EqualTo(-10));
                Assert.That(Parser.Parse("+10").Eval(), Is.EqualTo(10));
                Assert.That(Parser.Parse("--10").Eval(), Is.EqualTo(10));
                Assert.That(Parser.Parse("--++-+-10").Eval(), Is.EqualTo(10));
                Assert.That(Parser.Parse("10 + -20 - +30").Eval(), Is.EqualTo(-40));
            });
        }

        [Test]
        public void TestMulDivParens()
        {
            Assert.Multiple(() =>
            {
                Assert.That(Parser.Parse("10.2 + 20 / 2").Eval(), Is.EqualTo(20.2m));
                Assert.That(Parser.Parse("10.22 + 20 * 30").Eval(), Is.EqualTo(610.22m));
                Assert.That(Parser.Parse("(10 + 20) * 30").Eval(), Is.EqualTo(900));
                Assert.That(Parser.Parse("-(10 + 20) * 30").Eval(), Is.EqualTo(-900));
                Assert.That(Parser.Parse("-((10.555 + 20) * 5.3) * 30").Eval(), Is.EqualTo(-4858.245m));
            });
        }

        [Test]
        public void TestExceptions()
        {

            Assert.Multiple(() =>
            {
                Assert.That(() => Parser.Parse("10 + (20 * 30"), Throws.Exception
                    .TypeOf<ArgumentException>()
                    .With.Property("Message")
                    .EqualTo("Missing close parenthesis"));

                Assert.That(() => Parser.Parse("10 + (20 * a30)"), Throws.Exception
                    .TypeOf<InvalidDataException>()
                    .With.Property("Message")
                    .EqualTo("Unexpected character: a"));

            });
        }
    }
}