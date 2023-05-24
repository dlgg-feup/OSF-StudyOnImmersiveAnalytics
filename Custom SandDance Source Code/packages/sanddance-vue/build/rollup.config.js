import commonjs from '@rollup/plugin-commonjs';
import vue from 'rollup-plugin-vue';
import buble from '@rollup/plugin-buble';
import typescript from '@rollup/plugin-typescript';
import postcss from 'rollup-plugin-postcss';

export default {
    input: 'src/wrapper.js',
    output: {
        name: 'SandDanceVue',
        exports: 'named',
    },
    onwarn: function (warning) {
        // Suppress this error message... there are hundreds of them. Angular team says to ignore it.
        // https://github.com/rollup/rollup/wiki/Troubleshooting#this-is-undefined
        if (/The 'this' keyword is equivalent to 'undefined' at the top level of an ES module, and has been rewritten/.test(warning.message)) {
            return;
        }

        console.warn(warning.message);
    },
    plugins: [
        commonjs(),
        vue({
            css: true,
            compileTemplate: true,
        }),
        typescript(),
        postcss(),
        buble({
            transforms: {
                generator: false
            },
            exclude: [
                '**/*.css'
            ],
        }),
    ],
};
