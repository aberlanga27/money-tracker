import { useEffect, useId } from "react"

/**
 * LoadingBar component
 * @param {Object} props - Props for the component
 * @param {boolean} props.loading - Whether the loading bar should be shown
 * @param {number} props.step - The step size for the loading bar
 * 
 * @example
 * 
 * <LoadingBar loading={true} step={0.1} />
*/
export function LoadingBar({
    loading,
    step = 0.1,
}) {
    const uniqueId = useId()
    const componentId = `loading-bar-${uniqueId}`

     useEffect(() => {
        const loadingBar = document.getElementById(componentId)
        if (!loadingBar) return

        loadingBar.classList.toggle('opacity-0', loading)
     }, [componentId, loading])

    useEffect(() => {
        const loadingBar = document.getElementById(componentId)
        if (!loadingBar) return

        let width = 0
        const interval = setInterval(() => {
            width = (width >= 100) ? 0 : width + step
            loadingBar.firstChild.style.width = `${width}%`
        }, 10)

        return () => clearInterval(interval)
    }, [componentId, step])
    
    return (
        <div id={componentId} className="w-full bg-gray-200 rounded-full h-0.5 dark:bg-gray-200 my-0.5 opacity-0">
            <div className="bg-primary/90 h-0.5 rounded-full"></div>
        </div>
    )
}