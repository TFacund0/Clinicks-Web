# Responsive Conventions

Convenciones establecidas en la migración a responsive (Fase 1 — Infraestructura Compartida). Toda fase posterior que agregue o modifique markup responsive debe seguir estas reglas.

## 1. Mobile-first

Las clases sin prefijo apuntan al viewport más pequeño. Los prefijos `sm:`, `md:`, `lg:`, `xl:`, `2xl:` solo agregan o sobreescriben comportamiento para viewports iguales o más anchos que ese breakpoint. Nunca se debe partir de un layout de escritorio y "achicar" con media queries descendentes.

```jsx
// Correcto: mobile-first
<main className="p-4 md:p-6 lg:p-8">

// Incorrecto: no usar overrides descendentes
<main className="p-8 max-lg:p-4">
```

## 2. Breakpoints por defecto de Tailwind 4, sin valores custom

Se usan exclusivamente los breakpoints por defecto de Tailwind 4: `sm` (640px), `md` (768px), `lg` (1024px), `xl` (1280px), `2xl` (1536px). No se definen breakpoints custom en `tailwind.config` ni se usan valores arbitrarios (`min-[900px]:`) en el código.

## 3. `lg` (1024px) es el único breakpoint estructural

`lg` es el único punto de quiebre que cambia la estructura del layout (drawer off-canvas vs. sidebar estático). `md` y el resto de los breakpoints se usan únicamente para densidad de contenido (espaciado, tamaño de fuente, columnas), nunca para alternar entre patrones de navegación distintos.

## 4. CSS primero; hooks solo cuando el comportamiento no puede expresarse en CSS

La presentación (mostrar/ocultar, transformar, espaciar) se resuelve con clases de Tailwind (`hidden lg:flex`, `-translate-x-full lg:translate-x-0`). Los hooks de viewport (`useMediaQuery`, `useBreakpoint`) solo se usan cuando el comportamiento en JavaScript depende del viewport y no puede expresarse en CSS puro — por ejemplo, cerrar el drawer automáticamente al cruzar a `lg`, gestionar foco, o aplicar `inert`.

## 5. Orden de clases: layout → spacing → tipografía

Al escribir clases de Tailwind, ordenarlas de mayor a menor nivel de influencia estructural: primero layout (`flex`, `grid`, `fixed`, `static`), luego spacing (`p-4`, `gap-2`, `mt-4`), luego tipografía (`text-sm`, `font-bold`). Dentro de cada grupo, ordenar de menor a mayor breakpoint.

## 6. Importar `BREAKPOINTS`, no redeclarar valores en px

Cualquier código que necesite el valor numérico de un breakpoint debe importar `BREAKPOINTS` desde `src/hooks/useBreakpoint.js` en vez de declarar el número nuevamente:

```js
import { BREAKPOINTS } from '../hooks/useBreakpoint';

// Correcto
if (window.innerWidth >= BREAKPOINTS.lg) { /* ... */ }

// Incorrecto: no redeclarar el valor
if (window.innerWidth >= 1024) { /* ... */ }
```
