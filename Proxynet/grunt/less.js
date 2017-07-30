module.exports = {
  appDev: {
    files: {
      'build/app.css': ['src/styles/*.less', 'src/styles/**/*.less', 'src/pages/**/*.less']
    }
  },
  appProd: {
    files: {
      'build/app.css': ['src/styles/*.less', 'src/styles/**/*.less', 'src/pages/**/*.less']
    },
    options: {
      compress: true
    }
  }
};