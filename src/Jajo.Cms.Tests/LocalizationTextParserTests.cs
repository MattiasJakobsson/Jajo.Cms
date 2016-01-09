using System.Collections.Generic;
using Jajo.Cms.Endpoints;
using Jajo.Cms.Localization;
using Jajo.Cms.Parsing;
using Jajo.Cms.Rendering;
using Jajo.Cms.Theme;
using Should;

namespace Jajo.Cms.Tests
{
    public class LocalizationTextParserTests
    {
        private readonly LocalizationTextParser _parser;

        public LocalizationTextParserTests()
        {
            _parser = new LocalizationTextParser(new FakeLocalizer(), new FakeNamespaceFinder(), new FakeCultureFinder());
        }

        public void when_parsing_translation_with_existing_replacement()
        {
            var result = Parse("%[test replacements=replaceme:dsa]%");

            result.ShouldEqual("asd dsa asd");
        }

        private string Parse(string input)
        {
            return _parser.Parse(input,
                new DefaultCmsRenderer(new List<ICmsEndpoint>(), new List<ITextParser> { _parser }, new FakeEndpointConfigurationStorage()),
                new DefaultCmsContext(x => null, () => null, null, new List<ITheme> { new FakeTheme() }), new FakeTheme(), x => x);
        }
    }
}
