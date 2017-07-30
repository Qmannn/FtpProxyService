module.exports = {
  options: {
    encoding: 'utf8',
    algorithm: 'md5',
    length: 8
  },
  release: {
    src: [
        'build/*.{js,css}',
        'build/tlui/tlui.min.js',
        'build/tlui/tlui.min.css',
        'build/tlui/tlui-vendor.min.js',
        'build/tlui/monitoring.js'
    ]
  }
};