interface PageHeaderProps {
  title: string;
  description: string;
}

export function PageHeader({ title, description }: PageHeaderProps) {
  return (
    <div className="mb-8">
      <h1 className="mt-2 text-3xl font-black tracking-tight text-slate-950 dark:text-white sm:text-4xl">
        {title}
      </h1>
      <p className="mt-2 max-w-2xl text-slate-600 dark:text-slate-300">{description}</p>
    </div>
  );
}
