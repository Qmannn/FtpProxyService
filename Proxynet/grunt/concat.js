module.exports = {
  appJsDev: {
    files: {
      'build/app.js': ['src/dto/*.js',
        'src/dto/**/*.js',
        'src/app/*.js',
        'src/app/**/*.js',
        'src/pages/**/*.js',
        '.tmp/typescript.js',
        '.tmp/localization.js',
        '.tmp/templates.js'
      ]
    }
  }
};