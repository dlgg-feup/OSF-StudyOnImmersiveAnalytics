// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
import { ensureSearchExpressionGroupArray } from './group';
import { Search, SearchExpression, SearchExpressionGroup } from './types';

const expressionKeys = Object.keys(<Partial<SearchExpression>>{
    clause: null,
    name: null,
    operator: null,
    value: null
});

export function compareExpression(a: SearchExpression, b: SearchExpression): boolean {
    if (a && b) {
        for (let k = 0; k < expressionKeys.length; k++) {
            let key = expressionKeys[k];
            if (a[key] != b[key]) return false;
        }
    } else {
        return !a && !b;
    }
    return true;
}

const groupKeys = Object.keys(<Partial<SearchExpressionGroup>>{
    clause: null
});

export function compareGroup(a: SearchExpressionGroup, b: SearchExpressionGroup): boolean {
    for (let k = 0; k < groupKeys.length; k++) {
        let key = groupKeys[k];
        if (a[key] != b[key]) return false;
    }
    if (!a.expressions && !b.expressions) return true;
    if (!a.expressions || !b.expressions) return false;
    if (a.expressions.length != b.expressions.length) return false;
    for (let i = 0; i < a.expressions.length; i++) {
        if (!compareExpression(a.expressions[i], b.expressions[i])) return false;
    }
    return true;
}

export function compare(a: Search, b: Search): boolean {
    if (a == b) return true;
    if (!a || !b) return false;
    let arrs = [a, b].map(ensureSearchExpressionGroupArray);
    let [arrA, arrB] = arrs;
    if (arrA.length != arrB.length) return false;
    for (let i = 0; i < arrA.length; i++) {
        if (!compareGroup(arrA[i], arrB[i])) return false;
    }
    return true;
}

export function startsWith(whole: Search, part: Search): boolean {
    if (!part) return true;
    let arrs = [whole, part].map(ensureSearchExpressionGroupArray);
    let [wholeArray, partArray] = arrs;
    if (partArray.length > wholeArray.length) return false;
    for (let i = 0; i < partArray.length; i++) {
        if (!compareGroup(wholeArray[i], partArray[i])) return false;
    }
    return true;
}
