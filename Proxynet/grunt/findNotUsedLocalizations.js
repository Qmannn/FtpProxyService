module.exports = {
  tlApp: {
    files: {
      '.tmp/localization.js': [
        ['src/i18n/**/*.resx']
      ]
    },
    options: {
      srcFiles: [
          'src/**/*.tlui.xml',
          'src/**/*.html',
          'src/**/*.js',
          'src/**/*.ts'
      ]
    }
  }
};