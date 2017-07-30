var xml2js = require('xml2js');
var path = require('path');
var _ = require("underscore");

module.exports = function (grunt) {

    grunt.registerMultiTask('resx2angular', 'Convert resx to a angular constant definition file.', function () {

        var options = this.options({
            moduleName: 'angularModule',
            constantName: 'angularConstant'
        });

        var parser = new xml2js.Parser();
        var localizationStrings = {};
        var fileNameRegEx = /^(.+?)(\.(\w+))?\.resx$/g;
        this.files.forEach(function (file) {
            file.src.filter(function (filePath) {
                var baseName = path.basename(filePath);
                if (baseName.match(fileNameRegEx)) {
                    var fileName = RegExp.$1;
                    var lang = RegExp.$3 || 'ru';
                    var fileContent = grunt.file.read(filePath);
                    parser.parseString(fileContent, function (error, result) {
                        _.each(result.root.data, function (item) {
                            localizationStrings[fileName + '.' + item['$'].name + '.' + lang] = item.value[0];
                        });
                    });
                }
            });
            var localizationJson = JSON.stringify(localizationStrings, null, '\t');
            var output = "angular.module('"
                + options.moduleName
                + "').constant('"
                + options.constantName
                + "', "
                + localizationJson
                + ');';
            grunt.file.write(file.dest, output);
        });

    });
};