/// <binding BeforeBuild='tpBuild_dev' />
module.exports = function (grunt) {
    require('./grunt/common/tluiProjectBuilder')(grunt);
};