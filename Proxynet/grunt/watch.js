module.exports = {
  tlApp: {
    files: [
      ['src/dto/*.ts',
        'src/dto/**/*.ts',
        'src/app/Constants.ts',
        'src/app/Application.ts',
        'src/app/services/*.ts',
        'src/app/services/**/*.ts',
        'src/pages/**/*.ts'
      ],
      ['src/pages/**/*.html', 'src/pages/**/*.tlui.xml'],
      ['src/i18n/**/*.resx'],
      ['src/styles/*.less', 'src/styles/**/*.less', 'src/pages/**/*.less']
    ],
    tasks: ['build']
  }
};