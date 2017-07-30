module.exports = {
    // Common
    "tpDefault": [ "tpBuildDev", "watch" ],

    // Common Build Tasks
    "tpBuild_package": [ "clean:build", "tpCollectStatic", "tpBuildAppDev", "clean:tmp", "targethtml:package_dev", "targethtml:package_qa", "targethtml:package_prod", "tslint" ],
    "tpBuild_dev": [ "clean:build", "tpCollectStatic", "tpBuildAppDev", "clean:tmp", "targethtml:dev", "tslint" ],
    "tpBuild_qa": [ "clean:build", "tpCollectStatic", "tpBuildAppProd", "clean:tmp", "targethtml:qa", "tslint" ],
    "tpBuild_prod": [ "clean:build", "tpCollectStatic", "tpBuildAppProd", "clean:tmp", "targethtml:prod", "tslint" ],

    // Copy Static
    "tpCollectStatic": [ "copy:tlui", "copy:images", "copy:html" ],

    // Build App Dev
    "tpBuildAppDev": [ "tpBuildAppJsDev", "tpBuildAppCssDev", "tpBuildFileRev" ],
    "tpBuildAppCssDev": [ "less:appDev" ],
    "tpBuildAppJsDev": ["ngtemplates", "resx2angular", "ts", "concat:appJsDev", "ngAnnotate:tlApp"],

    // Build App Prod
    "tpBuildAppProd": [ "tpBuildAppJsProd", "tpBuildAppCssProd", "tpBuildFileRev" ],
    "tpBuildAppCssProd": [ "less:appProd" ],
    "tpBuildAppJsProd": [ "tpBuildAppJsDev", "uglify:appJs" ],

    // Build File Rev
    "tpBuildFileRev": [ "filerev", "usemin" ]
};