"use strict";

module.exports = function(grunt) {

  var Linter = require("tslint");

  grunt.registerMultiTask("tslint", "A linter for TypeScript.", function() {
    var options = this.options({
      formatter: "prose",
      outputFile: null
    });
    var done = this.async();
    var failed = 0;

    // Iterate over all specified file groups, async for 'streaming' output on large projects
    grunt.util.async.reduce(this.filesSrc, true, function(success, filepath, callback) {
      if (!grunt.file.exists(filepath)) {
        grunt.log.warn('Source file "' + filepath + '" not found.');
      } else {
        var contents = grunt.file.read(filepath);
        var linter = new Linter(filepath, contents, options);

        var result = linter.lint();

        if(result.failureCount > 0) {
          var outputString = "";
          var outputFile = options.outputFile;

          failed += result.failureCount;

          if (outputFile != null) {
            outputString = grunt.file.read(outputFile);
          }
          result.output.split("\n").forEach(function(line) {
            if(line !== "") {
              if (outputFile != null) {
                outputString += line + "\n";
              } else {
                grunt.log.write(line + '\r\n');
              }
            }
          });
          if(outputFile != null) {
            grunt.file.write(outputFile, outputString);
          }
          success = false;
        }
      }
      // Using setTimout as process.nextTick() doesn't flush
      setTimeout(function() {
        callback(null, success);
      }, 1);

    }, function(err, success) {
        if (err) {
            done(err);
        } else if (!success) {
            grunt.log.error(failed + " " + grunt.util.pluralize(failed,"error/errors") + " in " +
                            this.filesSrc.length + " " + grunt.util.pluralize(this.filesSrc.length,"file/files"));
            done(false);
        } else {
            grunt.log.ok(this.filesSrc.length + " " + grunt.util.pluralize(this.filesSrc.length,"file/files") + " lint free.");
            done();
        }
    }.bind(this));
  });

};
