using ShorderLink.Services;

namespace UnitTest
{
    public class TestURLShorter
    {
        [Fact]
        public void TestConstantLength()
        {
            string firtstShortUrl = URLShorter.HashString("https://smallUrl");
            string secondShortUrl = URLShorter.HashString("https://bigURl/dqjwdhjqhbvxznbvwuehi/eufhuwhfjbh&hh?cdahsjb");
            Assert.Equal(firtstShortUrl.Length, secondShortUrl.Length);
        }

        [Fact]
        public void TestSameShorterForSameLongURL()
        {
            string firtstShortUrl = URLShorter.HashString("https://smallUrl/mystbesame");
            string secondShortUrl = URLShorter.HashString("https://smallUrl/mystbesame");
            Assert.Equal(firtstShortUrl, secondShortUrl);
        }

        [Fact]
        public void TestDifferentShorter()
        {
            string firtstShortUrl = URLShorter.HashString("https://smallUrl/mystbesame");
            string secondShortUrl = URLShorter.HashString("https://smallUrl/mystbeNOTsame");
            Assert.NotEqual(firtstShortUrl, secondShortUrl);
        }
    }
}