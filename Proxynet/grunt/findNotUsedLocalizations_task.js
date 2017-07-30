var xml2js = require('xml2js');
var path = require('path');
var _ = require("underscore");

module.exports = function (grunt) {

    grunt.registerMultiTask('findNotUsedLocalizations', 'Find localizations which never used in sources.', function () {

        var options = this.options({
            srcFiles: 'srcPatterns'
        });

        var parser = new xml2js.Parser();
        var fileNameRegEx = /^(.+?)(\.(\w+))?\.resx$/g;
        var localizationStringRegEx = /T:[a-zA-Z0-9-_]+\.[a-zA-Z0-9-_]+/g;

        var usedLocalizations = {};
        grunt.file.expand(options.srcFiles).forEach(function (filePath) {
            var fileContent = grunt.file.read(filePath);
            var result = fileContent.match(localizationStringRegEx);
            if (result) {
                for (var i = 0; i < result.length; ++i) {
                    usedLocalizations[result[i]] = true;
                }
            }
        });

        var notUsedLocalizations = [];
        this.files.forEach(function (file) {
            file.src.filter(function (filePath) {
                var baseName = path.basename(filePath);
                if (baseName.match(fileNameRegEx)) {
                    var fileName = RegExp.$1;
                    var lang = RegExp.$3 || 'ru';
                    var fileContent = grunt.file.read(filePath);
                    parser.parseString(fileContent, function (error, result) {
                        _.each(result.root.data, function (item) {
                            var lName = 'T:' + fileName + '.' + item['$'].name;
                            if (!(lName in usedLocalizations)) {
                                notUsedLocalizations.push(lName + '.' + lang);
                            }
                        });
                    });
                }
            });
            
            if (notUsedLocalizations.length > 0) {
                grunt.warn('We found not used localizations:\n\n' + JSON.stringify(notUsedLocalizations, null, '  ') + '\n');
            }
        });
    });
};