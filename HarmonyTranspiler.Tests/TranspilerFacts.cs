using Xunit;

namespace HarmonyTranspiler.Tests
{
    public class TranspilerFacts
    {
        [Fact]
        public void TranspileShouldGetResult()
        {
            var transpiler = new Transpiler();
            var result = transpiler.Transpile("var foo = 23;");

            Assert.NotNull(result);
        }

        [Fact]
        public void TranspileShouldGetResultWithString()
        {
            var transpiler = new Transpiler();
            var result = transpiler.Transpile("var foo = 'bar';");

            Assert.NotNull(result);
        }

        [Fact]
        public void TranspileShouldGetResultWithDoubleQuotedString()
        {
            var transpiler = new Transpiler();
            var result = transpiler.Transpile("var foo = \"bar\";");

            Assert.NotNull(result);
        }

        [Fact]
        public void TranspileShouldHandleImport()
        {
            var transpiler = new Transpiler();
            var result = transpiler.Transpile(@"import foo from 'foo';");

            Assert.NotNull(result);
        }

        [Fact]
        public void TranspileShouldHandleExport()
        {
            var transpiler = new Transpiler();
            var result = transpiler.Transpile(@"export default {foo: 'bar'};");

            Assert.NotNull(result);
        }

        [Fact]
        public void TranspileShouldHandleLineFeed()
        {
            var transpiler = new Transpiler();
            var result = transpiler.Transpile("var foo = 'foo';\nvar bar = 'bar';");

            Assert.NotNull(result);
        }

        [Fact]
        public void TranspileShouldHandleCarriageReturnLineFeed()
        {
            var transpiler = new Transpiler();
            var result = transpiler.Transpile("var foo = 'foo';\r\nvar bar = 'bar';");

            Assert.NotNull(result);
        }

        [Fact]
        public void TranspileShouldSetModuleName()
        {
            var transpiler = new Transpiler("foo");

            var result = transpiler.Transpile("console.log('hi');");
            Assert.Contains("define(\"foo\",", result);
        }
    }
}
