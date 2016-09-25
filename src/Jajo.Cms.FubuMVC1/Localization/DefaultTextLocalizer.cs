using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using FubuCore;
using FubuCore.Configuration;
using FubuMVC.Core.Runtime.Files;
using Jajo.Cms.Localization;

namespace Jajo.Cms.FubuMVC1.Localization
{
    public class DefaultTextLocalizer : ILocalizeText
    {
        private static readonly IDictionary<string, string> Translations = new ConcurrentDictionary<string, string>();
        private static readonly object MissingLocker = new object();
        public const string MissingLocaleConfigFile = "missing.locale.config";
        private readonly IFileSystem _fileSystem = new FileSystem();
        private const string LeafElement = "string";
        private readonly IFubuApplicationFiles _fubuApplicationFiles;
        private readonly IEnumerable<ILocalizationVisitor> _visitors;

        public DefaultTextLocalizer(IFubuApplicationFiles fubuApplicationFiles, IEnumerable<ILocalizationVisitor> visitors)
        {
            _fubuApplicationFiles = fubuApplicationFiles;
            _visitors = visitors;
        }

        public virtual string Localize(string key, CultureInfo culture)
        {
            var translationKey = key;
            var writeMissing = true;
            var missingKeys = new List<string>();

            while (true)
            {
                var translationResult = GetTranslation(translationKey, culture);

                if (translationResult.Item2)
                    return _visitors.Aggregate(translationResult.Item1, (current, visitor) => visitor.AfterLocalized(translationKey, current));

                if (writeMissing)
                    missingKeys.Add(translationKey);

                var keyParts = translationKey.Split(':');

                var text = translationKey;

                if (keyParts.Length <= 1)
                {
                    WriteMissing(missingKeys, culture);

                    return text;
                }

                var namespaceParts = keyParts.Take(keyParts.Length - 1).ToArray();

                if (namespaceParts.Length <= 0)
                {
                    WriteMissing(missingKeys, culture);

                    return text;
                }

                var namespacePartsToUse = namespaceParts.Take(namespaceParts.Length - 1).ToArray();
                translationKey = keyParts.Last();

                if (namespacePartsToUse.Any())
                    translationKey = $"{string.Join(":", namespacePartsToUse)}:{translationKey}";

                writeMissing = false;
            }
        }

        public virtual void Load()
        {
            var fileSet = new FileSet
            {
                DeepSearch = false,
                Include = "*.locale.config",
                Exclude = MissingLocaleConfigFile
            };

            var directories = GetDirectoriesToSearch();

            directories.SelectMany(dir => _fileSystem.FindFiles(dir, fileSet)).Where(file =>
            {
                var fileName = Path.GetFileName(file);
                return fileName != null && !fileName.StartsWith("missing.");
            }).GroupBy(CultureFor).Each(group =>
            {
                var items = group.SelectMany(LoadFrom);

                foreach (var item in items)
                {
                    var key = BuildKey(group.Key, item.Item1);

                    Translations[key] = item.Item2;
                }
            });
        }

        public virtual IReadOnlyDictionary<string, string> GetTranslations(CultureInfo culture, string theme)
        {
            var translations = Translations
                .Where(x => x.Key.StartsWith($"{culture.Name.ToLower()}-"))
                .ToDictionary(x => x.Key.Substring($"{culture.Name.ToLower()}-".Length), x => x.Value);

            return new ReadOnlyDictionary<string, string>(translations);
        }

        protected virtual Tuple<string, bool> GetTranslation(string key, CultureInfo culture)
        {
            var translationKey = BuildKey(culture, key);

            if(Translations.ContainsKey(translationKey))
                return new Tuple<string, bool>(Translations[translationKey], true);

            return new Tuple<string, bool>("", false);
        }

        protected virtual string GetDefaultText(string key, CultureInfo culture)
        {
            return key;
        }

        protected virtual void WriteMissing(IEnumerable<string> keys, CultureInfo culture)
        {
            foreach (var key in keys)
                WriteMissing(key, culture);
        }

        protected virtual void WriteMissing(string key, CultureInfo culture)
        {
            var missingFileLocation = GetMissingLocaleFileLocation();

            lock (MissingLocker)
            {
                var missingDocument = GetMissingKeysDocument(missingFileLocation);

                if (missingDocument.DocumentElement?.SelectSingleNode("{0}[@{1}='{2}']".ToFormat((object)"missing", (object)"key", (object)key)) != null)
                    return;

                missingDocument.DocumentElement.AddElement("missing").WithAtt("key", key).WithAtt("culture", culture.Name).InnerText = "";
                missingDocument.Save(missingFileLocation);
            }
        }

        protected virtual IEnumerable<string> GetDirectoriesToSearch()
        {
            yield return _fubuApplicationFiles.GetApplicationPath();
        }

        private static CultureInfo CultureFor(string filename)
        {
            return new CultureInfo(Path.GetFileName(filename).Split('.').First());
        }

        private static XmlDocument GetMissingKeysDocument(string fileLocation)
        {
            return fileLocation.XmlFromFileWithRoot("missing-localization");
        }

        private string GetMissingLocaleFileLocation()
        {
            var directory = GetDirectoriesToSearch().FirstOrDefault();

            if (string.IsNullOrEmpty(directory))
                return null;

            return directory.AppendPath(MissingLocaleConfigFile);
        }

        private static IEnumerable<Tuple<string, string>> LoadFrom(string file)
        {
            var document = file.XmlFromFileWithRoot("jajo-localization");

            var xmlNodeList = document.DocumentElement?.SelectNodes(LeafElement);

            if (xmlNodeList == null) 
                yield break;

            foreach (XmlElement element in xmlNodeList)
                yield return new Tuple<string, string>(element.GetAttribute("key"), element.InnerText);
        }

        private static string BuildKey(CultureInfo culture, string key)
        {
            return $"{culture.Name.ToLower()}-{key}";
        }
    }
}