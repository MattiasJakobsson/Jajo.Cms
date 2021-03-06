using System.Collections.Generic;
using Jajo.Cms.Endpoints;
using Jajo.Cms.Parsing;
using Jajo.Cms.Rendering;
using Jajo.Cms.Theme;
using Should;

namespace Jajo.Cms.Tests
{
    public class MarkdowTextParserTests
    {
        private readonly MarkdowTextParser _parser;

        public MarkdowTextParserTests()
        {
            _parser = new MarkdowTextParser();
        }

        public void when_parsing_only_markdown_correct_html_is_returned()
        {
            var result = Parse("<md>##Mattias</md>");

            result.ShouldEqual("<h2>Mattias</h2>\n");
        }

        public void when_parsing_text_with_line_breaks_br_tags_should_be_added( )
        {
            var result = Parse("<md>asd\rdsa</md>");

            result.ShouldEqual("<p>asd<br />\ndsa</p>\n");
        }

        public void when_parsing_text_in_middle_of_text_correct_text_is_parsed()
        {
            var result = Parse("asd<md>#Mattias</md>dsa");

            result.ShouldEqual("asd<h1>Mattias</h1>\ndsa");
        }

        private string Parse(string input)
        {
            return _parser.Parse(input,
                new DefaultCmsRenderer(new List<ICmsEndpoint>(), new List<ITextParser> { _parser }, new FakeEndpointConfigurationStorage()),
                new DefaultCmsContext(x => null, () => null, null, new List<ITheme> { new FakeTheme() }), new FakeTheme(), x => x);
        }
    }
}