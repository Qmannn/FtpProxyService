module.exports = {
  tlApp: {
    files: {
      '.tmp/localization.js': [
        ['src/i18n/**/*.resx']
      ]
    },
    options: {
      moduleName: 'tlApp',
      constantName: 'tlApplicationLocalizationData'
    }
  }
};