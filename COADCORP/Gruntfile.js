/// <binding AfterBuild='default' />
module.exports = function (grunt) {

    // configuração do projeto
    grunt.initConfig({        
        less: {
            development: {
                options: {
                    paths: ['Content/themes/base']
                },
                files: [
                    {
                        expand: true,
                        src: ['*.less'],
                        dest: 'Content/themes/base',
                        cwd: 'Content/themes/base/less',
                        ext: '.css'
                    }         
                    
                ]
            }, 
            production: {
                options: {
                    paths: ['Content/themes/base']
                },
                files: [{
                    expand: true,
                    src: ['*.less'],
                    dest: 'Content/themes/base',
                    cwd: 'Content/themes/base/less',
                    ext: '.css'
                }]
            }
        },
        cssmin : {
            target: {
                options: {
                    sourceMap: true
                },
                files: [
                    {
                        expand: true,
                        src: ['**/*.css', '!**/*.min.css'],
                        dest: 'Content/themes/compilados',
                        cwd: 'Content/themes/',
                        ext: '.min.css'
                    }
                ]
            }
        },
        uglify: {
            my_target: {
                options: {
                    sourceMap: true
                },
                files: [
                    { src: 'Scripts/default.js', dest: 'Scripts/compilados/default.min.js' },
                    { src: 'Scripts/util.js', dest: 'Scripts/compilados/util.min.js' },
                    { src: 'Scripts/Funcoes.js', dest: 'Scripts/compilados/Funcoes.min.js' },
                    {
                        expand: true,
                        src: ['**/*.js', '!js/*.js'],
                        dest: 'Scripts/compilados/Angular/angular_modules',
                        cwd: 'Scripts/Angular/angular_modules',
                        ext: '.min.js'
                    },
                    {
                        expand: true,
                        src: ['**/*.js', '!js/*.js'],
                        dest: 'Scripts/compilados/negocio',
                        cwd: 'Scripts/negocio/',
                        ext: '.min.js'
                    },
                    {
                        expand: true,
                        src: ['**/*.js', '!js/*.js'],
                        dest: 'Scripts/compilados/appScripts',
                        cwd: 'Scripts/appScripts/',
                        ext: '.min.js'
                    }]
            },
        }
    });

    //Carregando o plugins que provê as taks do grunt
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-less');
    grunt.loadNpmTasks('grunt-contrib-cssmin');

    //Tarefa padrão
    grunt.registerTask('default', ['less', 'uglify', 'cssmin']);
}