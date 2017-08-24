module.exports = {
    // Common
    "Watch": [ "watch" ],
    "AfterBuild": ["ts"],
    "BeforeBuild": ["tslint"],

    "BuildVSCode": ["clean", "tslint", "ts", "ngtemplates", "filerev"]
};