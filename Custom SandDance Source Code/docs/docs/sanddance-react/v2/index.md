---
layout: docs
---

# @msrvida/sanddance-react

Visually explore, understand, and present your data.

![sanddance-animation](https://user-images.githubusercontent.com/11507384/54236654-52d42800-44d1-11e9-859e-6c5d297a46d2.gif)

[Demo](/app) - [API Reference](/docs/sanddance-react/v2/api)

## Installation

Add these to the `dependencies` section of your `package.json`, then run `npm install`:

```json
"@deck.gl/core": "6.4",
"@deck.gl/layers": "6.4",
"@msrvida/sanddance-react": "^2",
"luma.gl": "6.4",
"vega": "^5.8"
```

Import these in your JavaScript:

```js
import * as deck from '@deck.gl/core';
import * as layers from '@deck.gl/layers';
import * as luma from 'luma.gl';
import * as vega from 'vega';
import { SandDance, SandDanceReact } from '@msrvida/sanddance-react';

SandDance.use(vega, deck, layers, luma);
```

## For more information
Please visit the [SandDance website](/).
